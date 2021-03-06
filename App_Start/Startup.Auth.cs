﻿using System;
using System.Data.Entity.Migrations.Model;
using BombVacuum.Entity.Services;
using BombVacuum.Models;
using BombVacuum.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace BombVacuum
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public static string CookieType { get; private set; }
        public static string CookieName { get; private set; }
        
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            CookieType = DefaultAuthenticationTypes.ApplicationCookie;
            CookieName = "BombvacAuth";

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider

            //var type = typeof(CookieAuthenticationOptions)
            //    .Assembly.GetType("Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware");

            //app.Use(type, app, new CookieAuthenticationOptions
            //{
            //    TicketDataFormat =
            //        new SecureDataFormat<AuthenticationTicket>(DataSerializers.Ticket,
            //            new AesDataProtectorProvider("testing"), TextEncodings.Base64)
            //});


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieType,
                CookieName = CookieName,
                AuthenticationMode = AuthenticationMode.Active,
                ReturnUrlParameter = null,
                CookieHttpOnly = false,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: TimeSpan.FromMinutes(5),
                            regenerateIdentity:
                                (manager, user) =>
                                    user.GenerateUserIdentityAsync(manager, CookieType))
                }
            });
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Configure the application for OAuth based flow
            //PublicClientId = "self";
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/Token"),
            //    Provider = new ApplicationOAuthProvider(PublicClientId),
            //    AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    AllowInsecureHttp = true
            //};

            //// Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //if (SettingsService.Instance.GoogleOauth)
            //    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //    {
            //        ClientId = SettingsService.Instance.GoogleOauthId,
            //        ClientSecret = SettingsService.Instance.GoogleOauthSecret
            //    });
        }
    }
}
