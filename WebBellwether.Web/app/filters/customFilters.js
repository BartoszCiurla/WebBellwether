(function () {
    angular
        .module('customFilters', [])
        .filter('startFrom', function () {
            return function (input, start) {
                start = +start; //parse to int
                return input.slice(start);
            }
        })
    .filter('filterByIdTakeWhenNotExists', function () {
        return function (input, value) {
            var result = [];
            input.forEach(function (item) {
                if (item.id !== value) {
                    result.push(item);
                }
            });
            return result;
        };
    });

})();