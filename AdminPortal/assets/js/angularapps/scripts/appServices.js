ngSpaApp.service('ngSpaAppService', ["$http", function ($http){
    return {
        load: function (year) {
            return $http.get("/webapi/yearly-wage-exp/get-table-data/" + year);
        },

        addNewEmployee: function (jsonfiedData) {
            return $http.post("/webapi/yearly-wage-exp/quick-add-new-employee", jsonfiedData);
        },

        quickAddNewEntry: function (jsonfiedData, year) {
            return $http.post("/webapi/yearly-wage-exp/quick-add-new-entry/" + year, jsonfiedData);
        },

        getEmployeeList: function () {
            return $http.get("/webapi/yearly-wage-exp/get-employee-list-for-new-entry");
        },

        saveEntryAmendments: function (jsonfiedData) {
            return $http.put("/webapi/yearly-wage-exp/put-entry-details-for-edit", jsonfiedData);
        },

        deleteEntry: function (yearId) {
            return $http.delete("/webapi/yearly-wage-exp/delete/" + yearId);
        }
    }
}]);