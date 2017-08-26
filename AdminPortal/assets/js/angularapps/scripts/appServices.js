ngSpaApp.service('ngSpaAppService', ["$http", function ($http){
    return {
        load: function (year) {
            return $http.get("/webapi/yearly-wage-exp/get-table-data/" + year);
        }
    }
}]);