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
const auth = (req, res, next) => {
    if (!req.isAuthenticated()) return res.redirect('/');
    return next();
}

module.exports = {
    checkMimeType,
    uploadFile,
    auth
};
