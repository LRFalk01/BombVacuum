'use strict';

cbApp.controller('SignalrController', ['$scope',
    function SignalrController($scope) {
        var self = this;


        self.Init = function () {
            $scope.hub = $.connection.testHub;
            $scope.hub.client.hello = function(time) {
                $scope.hubTime = time;
                $scope.transport = $scope.hub.connection.transport.name;
                $scope.$apply();
            };

            $scope.hub.client.currentUser = function (user) {
                $scope.currentUser = user;
                $scope.$apply();
            };

            $.connection.hub.start().done(function() {
                $scope.hub.server.hello();
                $scope.hub.server.currentUser();
            });
        };
        self.Init();
    }]
);