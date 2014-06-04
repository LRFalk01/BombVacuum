'use strict';

//service to wrap the standard usage of $http within services
//wraps the request in a $q promise
cbApp.factory('HttpRequestService', function ($http, $log, $q) {
    return {
        Go: function (httpOptions) {
            var deferred = $q.defer();
            $http(httpOptions)
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                $log.warn("Error: ", data);
                deferred.reject(data);
            });
            return deferred.promise;
        }
    };
});