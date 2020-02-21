const auth = (req, res, next) => {
    if (!req.isAuthenticated()) return res.redirect('/');
    return next();
};
const checkMimeType = (mimeType) => {
    const mimeTypes = [
        'text/plain',
        'application/msword',
        'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
        'application/pdf',
        'application/vnd.oasis.opendocument.text'
    ];
    return mimeTypes.includes(mimeType);
};
const uploadFile = (s3, file, cb) => {
    const config = require('./config.json');
    const params = {
        Bucket: config.bucketName,
        Key: file.name,
        Body: Buffer.from(file.data, 'binary')
    };
    s3.upload(params, cb);
};
// const getItem  = require('util').promisify()
const getDynamoDBData = async () => {
    const AWS = require('aws-sdk');
    const config = require('./config.json');
    AWS.config.loadFromPath('./aws-config.json');

    let docClient = new AWS.DynamoDB.DocumentClient();
    const params = { TableName: config.dynamoDBTable };
    const scan = (p) => {
        return new Promise((res, rej) => {
            docClient.scan(p, (err, data) => {
                if (!err) return res(data);
                return rej(err)
            });
        });
    };
    // const a = await get(getParams);
    const a = await scan(params);
    return a;
};

module.exports = {
    auth,
    checkMimeType,
    uploadFile,
    getDynamoDBData
};
