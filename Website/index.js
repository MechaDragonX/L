const express = require('express');
const { S3 } = require('aws-sdk');
const fileUpload = require('express-fileupload');
const exphbs = require('express-handlebars');
const session = require('express-session');
const app = express();
const { lookup } = require('mime-types');
const passport = require('passport')
const GoogleStrategy = require('passport-google-oauth').OAuth2Strategy;

const { checkMimeType, uploadFile, auth, scanDynamoDB, getFromDynamoDB, checkParams } = require('./util');
const { mergeSort, merge, sort, sortId, sortSurname, sortGivenName, sortEmail } = require('./sortinghelper');
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
app.engine('hbs', exphbs({ 
    extname:'hbs',
    helpers: require('./handlebars-helpers')
}));
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
        return res.render('index', { title: 'Login' });
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
app.get('/applicants', auth, async (req, res) => {
    let data = await scanDynamoDB();
    data = data.Items;
    console.log(data);
    // TODO: Sort data by surname
    // data = data.Items.sort();
    // console.log(data);
    return res.render('data', { title: 'Applicant List', data, detailed: false });
});
app.get('/applicants/:params', async (req, res) => {
    let unsorted = await scanDynamoDB();
    unsorted = unsorted.Items;

    if(checkParams(req.params.params)) {
        let params = req.params.params.split('-');
        let detail = unsorted.find(x => (parseInt(params[0]) === x.id) && (params[1] === x.surname));
        console.log(detail);
        return res.render('data', { title: 'Applicant List', data: unsorted, detail: detail, detailed: true });
    }

    let sorted = sort(unsorted, req.params.params);
    return res.render('data', { title: 'Applicant List', data: sorted });
});
app.get('/applicants/details/:id_surname', async (req, res) => {
    console.log('blarg!');
    let params = req.params.id_surname.split('-');
    let applicant = await getFromDynamoDB(parseInt(params[0]), params[1]);
    applicant = applicant.Item;
    let title = 'More details for ' + applicant.surname + ', ' + applicant.givenName;

    return res.render('detail', { title: title, applicant: applicant });
});
app.get('*', (req, res) => {
    return res.render('404', { layout: 'error.hbs', title: '404: Resource not Found' });
})
app.listen(config.port, () => {
    console.log(`Running on ${process.env.NODE_ENV === 'production' ? 'production' : 'debug'}, listening on port ${config.port}`);
});
