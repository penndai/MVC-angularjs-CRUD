var customerApp = angular.module("customerApp", ["ngRoute","ngResource"]).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: 'list.html' }).
            when('/add', { controller: AddCtrl, templateUrl: 'new.html' }).
            when('/edit/:id', { controller: EditCtrl, templateUrl: 'new.html' }).
            otherwise({ redirectTo: '/' });
    });

customerApp.factory('CustomerService', function ($resource) {
    return $resource('api/Customer/:customerid', { customerid: '@id' }, { update: {method:'PUT'}});
});


var AddCtrl = function ($scope, $location, CustomerService) {
    $scope.action = "Add";
    $scope.save = function () {
        CustomerService.save($scope.item, function () {
            $location.path('/');
        });
    };
};

var EditCtrl = function ($scope, $location, $routeParams, CustomerService) {
    $scope.action = "Edit";
    var updatedItem = CustomerService.get({ id: $routeParams.id });
    $scope.item = updatedItem;

    //non-GET "class" actions: Resource.action([parameters], postData, [success], [error])
    $scope.save = function () {       
        CustomerService.update({id:$routeParams.id},updatedItem, function () {            
            $location.path('/');
        });
    };
};

var ListCtrl = function ($scope, $location, CustomerService) {
    $scope.test = 'Get all customers';
    $scope.more = true;
    $scope.limit = 20;
    $scope.offset = 0;

    $scope.customers = [];

    $scope.hasmore = function () {
        return $scope.more;
    };   

    $scope.delete = function () {
        var id = this.c.ID;
        CustomerService.delete({ id: id }, function () { $("#customer_" + id).fadeOut();});
    };

    // define the query to MVC controller
    $scope.search = function () {
        CustomerService.query({ sort: $scope.orderby, desc: $scope.desc, limit: $scope.limit, offset: $scope.offset },
        function (data) {
            $scope.more = data.length == 5;

            $scope.customers = data;
        });
    };

    // define the sort method
    $scope.sort = function (order) {
        if ($scope.orderby == order) {
            $scope.desc = !$scope.desc;
        }
        else {
            $scope.desc = true;
            $scope.orderby = order;
        }

        $scope.search();
    };
    
    
    $scope.search();
}