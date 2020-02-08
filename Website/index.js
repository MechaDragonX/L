const express = require('express');
const {S3} = require('aws-sdk');
const fileUpload = require('express-fileupload');
const exphbs = require('express-handlebars');
const app = express();
const {lookup} = require('mime-types');
const {checkMimeType, uploadFile} = require('./util');
const config = require('./config.json');
const s3 = new S3({
    accessKeyId: config.accessKey,
    secretAccessKey: config.secretAccessKey
});

app.use(express.static('views'));
app.use(express.static('styles'));
app.use(fileUpload());
app.engine('hbs', exphbs({ extname:'hbs' }));
app.set('view engine', 'hbs');

app.get('/', (req, res) => {
    return res.render('index', { title: 'Resume Upload Tool' });
});

app.post('/', (req, res) => {
    if(!checkMimeType(lookup(req.files.resume.name)))
        return res.render('index', { title: 'Resume Upload Tool', error: true, message: 'Unsupported File Type!' });
    uploadFile(s3, req.files.resume, (err, data) => {
        if(err)
            return res.render('index', { title: 'Resume Upload Tool', error: true, message: 'Something went Wrong!' });
        return res.render('index', { title: 'Resume Upload Tool', success: true, data });
    });
});

app.get('*', (req, res) => {
    res.render('404');
})

app.listen(config.port, () => {
    console.log(`listening on port ${config.port}`);
});
