'use strict';


// Declare app level module which depends on filters, and services
var cbApp = angular.module('cbApp', ['ui.router', 'ui.validate', 'cgBusy'])
    .config(['$urlRouterProvider', '$stateProvider', '$locationProvider', function ($urlRouterProvider, $stateProvider, $locationProvider) {
        $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise('/');

        var routes = new Array();
        routes.push({
            name: 'root',
            url: '/',
            //controller: 'RootController',
            templateUrl: '/assets/app/root/layout.html',
            permission: null
        });
        routes.push({
            name: 'test',
            parent: 'root',
            url: 'testurl',
            //controller: 'AccountSettingsController',
            template: 'Test Child Route',
            permission: null
        });

        for (var route in routes) {
            $stateProvider.state(routes[route]);
        }
    }]);


