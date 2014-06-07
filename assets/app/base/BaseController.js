'use strict';

cbApp.controller('BaseController', ['$scope', '$log', '$state', 'CurrentUserService',
    function BaseController($scope, $log, $state, currentUserService) {
        var self = this;
        $scope.CurrentUserService = currentUserService;

        $scope.IsAuthenticated = currentUserService.State.IsAuthenticated;
        $scope.CurrentUser = currentUserService.State.CurrentUser;

        $scope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            if (toState.name != 'login' && !currentUserService.State.IsAuthenticated) {
                event.preventDefault();
                $state.go('login');
                return;
            }

            if ( !self.AuthRoute(toState)) {
                event.preventDefault();
                $state.go('base');
            }
        });

        self.AuthRoute = function (route) {
            var routePath = new Array();
            var hasParent = true;
            while (hasParent) {
                if (route == null) return false;

                routePath.unshift(route);
                if (route.parent == undefined) {
                    hasParent = false;
                    continue;
                }
                route = $state.get(route.parent);
            }
            for (var i = 0; i < routePath.length; i++) {
                var curRoute = routePath[i];
                var permission = curRoute.permission;
                if (permission != undefined && !currentUserService.InRole(permission)) {
                    return false;
                }
            }
            return true;
        };

        $scope.LogOff = function () {
            currentUserService.LogOff()
                .then(function () {
                    currentUserService.GetCurrentUser().then(function () {
                        $scope.IsAuthenticated = currentUserService.State.IsAuthenticated;
                        $scope.CurrentUser = currentUserService.State.CurrentUser;
                        $state.go("base");
                    });
                });
        };

        $scope.Login = function (username, password) {
            return currentUserService.Login({ Email: username, Password: password })
            .then(function (data) {
                    currentUserService.GetCurrentUser().then(function () {
                    $scope.IsAuthenticated = currentUserService.State.IsAuthenticated;
                    $scope.CurrentUser = currentUserService.State.CurrentUser;
                        $state.go("base");
                    });
                });
        };

        self.Init = function () {
            if (!currentUserService.State.IsAuthenticated) {
                $state.go('login');
                return;
            }

            if (!self.AuthRoute($state.current)) {
                $state.go('base');
                return;
            }
        };
        self.Init();
    }]
);