using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;

using L;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace L.Lambda
{
    public class Parser
    {
        private static AmazonDynamoDBClient DDBClient = new AmazonDynamoDBClient();

        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Parser()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Parser(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> S3EventHandler(S3Event evnt, ILambdaContext context)
        {
            var s3Event = evnt.Records?[0].S3;
            if(s3Event == null)
            {
                return null;
            }

            try
            {
                string decodedKey = HttpUtility.UrlDecode(s3Event.Object.Key);
                context.Logger.LogLine($"Reading \"{decodedKey}\"...");
                Stream stream = await S3Client.GetObjectStreamAsync(s3Event.Bucket.Name, decodedKey, null);
                string applicantJSON = ParseDataFromS3(stream, decodedKey);
                context.Logger.LogLine($"\"{decodedKey}\" read\n{applicantJSON}");
                context.Logger.LogLine($"Putting data in DynamoDb Table...");
                await PutDataInDynamoDB(applicantJSON);
                context.Logger.LogLine($"Data put successfully! Check the DynamoDB table for more details.");
                return "Success!";
            }
            catch (Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }
        private static string ParseDataFromS3(Stream stream, string key)
        {
            string[] lines = FileParser.ExtractAllLinesFromS3(stream, key);
            ResumeParser resumeParser = new ResumeParser(lines);
            Applicant applicant = resumeParser.Parse();
            applicant.GenerateID();
            return applicant.ToString();
        }
        private static async Task PutDataInDynamoDB(string applicantJSON)
        {
            Document item = Document.FromJson(applicantJSON);
            Table table = Table.LoadTable(DDBClient, "Applicants");
            await table.PutItemAsync(item);
        }
    }
}
