(function () {
    angular
        .module('customFilters', [])
        .filter('startFrom', function () {
            return function (input, start) {
                if (input) {
                    start = +start;
                    return input.slice(start);
                }
                return [];
            };
        })
    .filter('filterByIdTakeWhenNotExists', function () {
        return function (input, value) {
            var result = [];
            input.forEach(function (item) {
                if (item.Id !== value) {
                    result.push(item);
                }
            });
            return result;
        };
    });

})();