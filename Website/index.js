const express = require('express');
const { S3 } = require('aws-sdk');
const fileUpload = require('express-fileupload');
const exphbs = require('express-handlebars');
const session = require('express-session');
const app = express();
const { lookup } = require('mime-types');
const passport = require('passport')
const GoogleStrategy = require('passport-google-oauth').OAuth2Strategy;

const {checkMimeType, uploadFile, auth, scanDynamoDB, getFromDynamoDB } = require('./util');
const config = require('./config.json');

const s3 = new S3({
    accessKeyId: config.accessKey,
    secretAccessKey: config.secretAccessKey
});


app.use(session({ 
    secret: 'dfog8nyro893mof23yijertnio',
    resave: false,
    saveUninitialized: true
}));
app.use(passport.initialize());
        app.use(passport.session());
app.use(express.static('styles'));
app.use(fileUpload());
app.engine('hbs', exphbs({ extname:'hbs' }));
app.set('view engine', 'hbs');

passport.use(new GoogleStrategy({
    clientID: config.googleOAuthClientID,
    clientSecret: config.googleOAuthClientSecret,
    callbackURL: process.env.NODE_ENV === 'production' ? config.production.googleOAuthCallbackURL : config.debug.googleOAuthCallbackURL
  },
    function(accessToken, refreshToken, profile, done) {
        return done(null, profile);
    }
));
passport.serializeUser(function(user, done) {
    done(null, user);
});
passport.deserializeUser(function(user, done) {
    done(null, user);
});

app.get('/auth/google', passport.authenticate('google', {
    scope: ['profile'],
    failureRedirect: '/'
}));

app.get('/auth/google/callback', passport.authenticate('google'), (req, res) => {
    res.redirect('/upload');
});
app.get('/(|login)', (req, res) => {
    if(!req.isAuthenticated())
        return res.render('index', { title: 'Login' });45
});
app.get('/upload', auth, (req, res) => {
    return res.render('upload', { title: 'Resume Upload Tool', user: req.user ? req.user : null });
});
app.post('/upload', (req, res) => {
    if(!checkMimeType(lookup(req.files.resume.name)))
        return res.render('upload', { title: 'Resume Upload Tool', error: true, message: 'Unsupported File Type!' });
    uploadFile(s3, req.files.resume, (err, data) => {
        if(err)
            return res.render('upload', { title: 'Resume Upload Tool', error: true, message: 'Something went Wrong!' });
        return res.render('upload', { title: 'Resume Upload Tool', success: true, data });
    });
});
app.get('/data', async (req, res) => {
    let data = await scanDynamoDB();
    data = data.Items;
    // TODO: Sort data by surname
    // data = data.Items.sort();
    // console.log(data);
    return res.render('data', { title: 'Data', data, detailed: false });
});
app.get('/data/:id_surname', async (req, res) => {
    let params = req.params.id_surname.split('-');
    let detail = await getFromDynamoDB(params[0], params[1]);
    detail = detail.Item;

    let data = await scanDynamoDB();
    data = data.Items;

    return res.render('data', { title: 'Data', data: data, detail: detail, detailed: true });
});
app.get('*', (req, res) => {
    return res.render('404', { layout: 'error.hbs', title: '404: Resource not Found' });
})
app.listen(config.port, () => {
    console.log(`Running on ${process.env.NODE_ENV === 'production' ? 'production' : 'debug'}, listening on port ${config.port}`);
});
