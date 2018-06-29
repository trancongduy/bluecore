export var authConfig = {
    // Url of the Identity Provider
    issuer: 'http://localhost:5000',
    loginUrl: 'http://localhost:5000/connect/authorize',
    // URL of the SPA to redirect the user to after login
    //redirectUri: 'http://localhost:8100/signin-oidc',
    redirectUri: window.location.origin + '/index.html',
    postLogoutRedirectUri: 'http://localhost:8100',
    requestAccessToken: true,
    requireHttps: false,
    // The SPA's id. The SPA is registerd with this id at the auth-server
    clientId: 'mvc',
    dummyClientSecret: 'secret',
    // set the scope for the permissions the client should request
    // The first three are defined by OIDC. The 4th is a usecase-specific one
    scope: 'openid profile api1 offline_access',
};
//# sourceMappingURL=auth.config.js.map