const express = require('express');
const {S3} = require('aws-sdk');
const fileUpload = require('express-fileupload');
const exphbs = require('express-handlebars');
const app = express();
app.use(express.static('views'));
app.use(express.static('styles'));
const config = require('./config.json');

const s3 = new S3({
    accessKeyId: config.accessKey,
    secretAccessKey: config.secretAccessKey
});

const uploadFile = (file, cb) => {
    const params = {
        Bucket: config.bucketName,
        Key: file.name,
        Body: Buffer.from(file.data, 'binary')
    };
    s3.upload(params, cb);
};

app.engine('hbs', exphbs({extname:'hbs'}));
app.set('view engine', 'hbs');
app.use(fileUpload());  

app.get('/', (req, res) => {
    return res.render('index')
});

app.post('/', (req, res) => {
    uploadFile(req.files.resume, (err, data) => {
        if(err == null) {
            return res.render('success', { data });
        }
        else {
            console.log(err);
            return res.render('error');
        }
    });
});

app.listen(config.port, () => {
    console.log(`listening on port ${config.port}`);
});
