'use strict';


// Declare app level module which depends on filters, and services
var cbApp = angular.module('cbApp', ['ui.router', 'ui.validate', 'cgBusy'])
    .config(['$urlRouterProvider', '$stateProvider', '$locationProvider', function ($urlRouterProvider, $stateProvider, $locationProvider) {
        $locationProvider.html5Mode(false);

        $urlRouterProvider.otherwise('/');

        var routes = new Array();
        routes.push({
            name: 'base',
            url: '/',
            controller: 'BaseController',
            templateUrl: '/assets/app/base/layout.html',
            permission: null,
            resolve: {
                CurrentUserService: [
                    'CurrentUserService', function(currentUserService) {
                        return currentUserService.GetCurrentUser().then(function() {
                            return currentUserService;
                        });
                    }
                ]
            }
        });
        routes.push({
            name: 'login',
            parent: 'base',
            url: 'login',
            controller: 'LoginController',
            templateUrl: '/assets/app/account/login.html',
            permission: null
        });
        routes.push({
            name: 'register',
            parent: 'base',
            url: 'register',
            controller: 'RegisterController',
            templateUrl: '/assets/app/account/register.html',
            permission: null
        });
        routes.push({
            name: 'game',
            parent: 'base',
            url: 'game',
            controller: 'GameController',
            templateUrl: '/assets/app/game/Game.html',
            permission: null
        });

        for (var route in routes) {
            $stateProvider.state(routes[route]);
        }
    }]);


