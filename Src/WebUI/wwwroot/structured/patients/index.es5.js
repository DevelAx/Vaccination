"use strict";

(function () {
    activateSearch();

    function activateSearch() {
        var timeout,
            intervalId,
            searchId = 0;
        var page = 1;
        var $pageInfo = $("#page-info");
        var $searchState = $("#searchState");
        var $searchProgress = $("#searchProgress");
        var getPatientsUrl = document.querySelector("#getPatientsUrl").value;
        var $patientsListContainer = $("#patients-list");
        var $paginationContainer = $("#pagination");
        var $fullName = document.querySelector('#fullNameSearch');
        var $insuranceNumber = document.querySelector('#insuranceNumberSearch');

        onChange($fullName, onSearchTextChanged);
        onChange($insuranceNumber, onSearchTextChanged);
        requestSearch(); // The very first search is initiated automatically.

        function onChange($element, callback) {
            $element.addEventListener('input', callback);
            $element.addEventListener('propertychange', callback); // IE8

            $($element).keypress(function (e) {
                var keycode = e.keyCode ? e.keyCode : e.which;
                if (keycode === 13) {
                    clearTimeout(timeout);
                    requestSearch();
                }
            });
        }

        function onSearchTextChanged() {
            clearTimeout(timeout);
            timeout = setTimeout(requestSearch, 2000);
        }

        function requestSearch() {
            startSearchProgress();

            var urlParams = {
                fullName: $fullName.value,
                insuranceNumber: $insuranceNumber.value,
                searchId: searchId++,
                page: page - 1
            };

            $.get(getPatientsUrl, urlParams).done(function (data) {
                if (data.searchId !== searchId - 1) return;

                stopSearchProgress(data.patients.length);
                page = data.pageNumber + 1;
                $patientsListContainer.mirandajs(data.patients);
                $paginationContainer.mirandajs(data.pages.length < 2 ? [] : data.pages);
                updatePageInfo(data);
                makeClickableRows();
                subscribeToPagination();
            });
        }

        function startSearchProgress() {
            $searchProgress.css("color", "red");
            $searchState.text("Отправка запроса ");
            $searchProgress.text("");

            intervalId = setInterval(function () {
                var text = $searchProgress.text();

                if (text.length < 8) $searchProgress.text(text + '.');else $searchProgress.text('');
            }, 250);
        }

        function stopSearchProgress(foundCount) {
            $searchProgress.css("color", "black");
            clearInterval(intervalId);
            $searchProgress.text("");
            $searchState.text("Найдено: " + foundCount);
        }

        function updatePageInfo(data) {
            $pageInfo.text("Страница " + (data.pageNumber + 1) + " из " + data.pages.length);
        }

        function makeClickableRows() {
            $(".clickable-row").click(function () {
                window.location = $(this).data("href");
            });
        }

        function subscribeToPagination() {
            var $curPage = $(".page-item:nth-child(" + page + ")");
            $curPage.addClass("disabled");

            $(".page-item").click(function () {
                var $anchor = $(this);
                page = parseInt($anchor.text());
                requestSearch();
            });
        }
    }
})();

