export const IDENTITY_CONFIG = {
    authority: process.env.REACT_APP_AUTH_URL, //(string): The URL of the OIDC provider.
    client_id: process.env.REACT_APP_IDENTITY_CLIENT_ID, //(string): Your client application's identifier as registered with the OIDC provider.
    redirect_uri: process.env.REACT_APP_REDIRECT_URL, //The URI of your client application to receive a response from the OIDC provider.
    login: process.env.REACT_APP_AUTH_URL + "/login",
    post_logout_redirect_uri: process.env.REACT_APP_LOGOFF_REDIRECT_URL, // (string): The OIDC post-logout redirect URI.
    audience: "https://menuids.com/authorize", //is there a way to specific the audience when making the jwt
    response_type: "code", //(string, default: 'id_token'): The type of response desired from the OIDC provider.
    scope: "openid profile idsApi IdentityServerApi", //(string, default: 'openid'): The scope being requested from the OIDC provider.
};

export const METADATA_OIDC = {
    issuer: "https://localhost:5006",
    jwks_uri: process.env.REACT_APP_AUTH_URL + "/.well-known/openid-configuration/jwks",
    authorization_endpoint: process.env.REACT_APP_AUTH_URL + "/connect/authorize",
    token_endpoint: process.env.REACT_APP_AUTH_URL + "/connect/token",
    userinfo_endpoint: process.env.REACT_APP_AUTH_URL + "/connect/userinfo",
    end_session_endpoint: process.env.REACT_APP_AUTH_URL + "/connect/endsession",
    check_session_iframe: process.env.REACT_APP_AUTH_URL + "/connect/checksession",
    revocation_endpoint: process.env.REACT_APP_AUTH_URL + "/connect/revocation",
    introspection_endpoint: process.env.REACT_APP_AUTH_URL + "/connect/introspect"
};