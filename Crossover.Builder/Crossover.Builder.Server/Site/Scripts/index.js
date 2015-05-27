var tokenKey = 'accessToken';
function getToken() {
    var token = sessionStorage.getItem(tokenKey);
    return token;
}

function getData(url, done, fail) {
    var result;
    var token = getToken();
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    $.ajax({
        type: 'GET',
        url: url,
        headers: headers,
        success: done,
        error: fail
    }).done(function(data) {
        result = data;
    });
    return result;
}

function getUser() {
    var user = getData('/api/account/user');
    return user;
}

function ViewModel() {
    var self = this;


    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();

    function showError(jqXHR) {
        self.result(jqXHR.status + ': ' + jqXHR.statusText);
    }

    self.callApi = function () {
        self.result('');

        var token = getToken();
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/values/5',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    }

    self.register = function () {
        self.result('');

        var data = {
            Email: self.registerEmail(),
            Password: self.registerPassword(),
            ConfirmPassword: self.registerPassword2()
        };

        $.ajax({            
            type: 'POST',
            url: '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        }).done(function (data) {
            self.result("Done!");
        }).fail(showError);
    }

    self.login = function () {
        self.result('');

        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).done(function (data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
        }).fail(showError);
    }

    self.logout = function () {
        self.user('');
        sessionStorage.removeItem(tokenKey);
    }

    self.showRegisterOrLogin = function () {
        $('#registerForm').toggleClass('collapse');
        $('#loginForm').toggleClass('collapse');
    }
}

var app = new ViewModel();
ko.applyBindings(app);

$(document).ready(function () {
    var user = getUser();
    if(user)
        $('h2').css({ "color": 'red' });
    else
        $('h2').css({ "color": 'blue' });
});