'use strict';


// Declare app level module which depends on filters, and services
var cbApp = angular.module('cbApp', ['ui.router', 'ui.validate', 'cgBusy'])
    .config(function ($urlRouterProvider, $stateProvider) {
        $urlRouterProvider.otherwise('/');

        var routes = new Array();
        routes.push({
            name: 'root',
            url: '/',
            //controller: 'RootController',
            templateUrl: '/assets/app/root/layout.html',
            permission: null
        });
        //routes.push({
        //    name: 'account-settings',
        //    parent: 'root',
        //    url: 'account-settings',
        //    controller: 'AccountSettingsController',
        //    templateUrl: 'app/account-settings/account-settings.html',
        //    permission: null
        //});

        for (var route in routes) {
            $stateProvider.state(routes[route]);
        }
    });


