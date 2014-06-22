'use strict';

cbApp.factory('CurrentUserService', ['$log', 'HttpRequestService', function($log, HttpRequestService) {
        var self = this;
        self.State = {};
        self.State.IsAuthenticated = false;
        self.State.CurrentUser = null;
        self.State.Roles = [];

        self._getCurrentUserPromise = undefined;
        self._GetCurrentUser = function() {
            if (self._getCurrentUserPromise != undefined) return self._getCurrentUserPromise;

            var _getCurrentUserPromise = HttpRequestService.Go({
                    method: "GET",
                    url: '/API/Account/CurrentUser',
                    withCredentials: true
                })
                .then(function(data) {
                    self.State.IsAuthenticated = data.IsAuthenticated;
                    self.State.CurrentUser = null;
                    self.State.Roles = [];
                    if (self.State.IsAuthenticated) {
                        self.State.CurrentUser = data;
                        self.State.Roles = data.Roles;
                    }
                });
            return _getCurrentUserPromise;
        };

        return {
            State: self.State,
            InRole: function(roleName) {
                if (roleName == undefined || self.State.Roles == undefined) return undefined;
                return self.State.Roles.indexOf(roleName) != -1;
            },
            GetCurrentUser: self._GetCurrentUser,
            Login: function(data) {
                return HttpRequestService.Go({
                    method: "POST",
                    url: '/API/Account/Login',
                    data: JSON.stringify(data),
                    withCredentials: true
                });
            },
            LogOff: function() {
                return HttpRequestService.Go({
                    method: "POST",
                    url: '/API/Account/Logout',
                    withCredentials: true
                })
                .then(function() {
                    self.State.CurrentUser = null;
                    self.State.IsAuthenticated = false;
                });
            }
        };
    }
]);