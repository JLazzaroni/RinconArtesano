; (function (window) {

    'use strict';

    var mainContainer = document.querySelector('.main-wrap'),
		openCtrl = document.getElementById('btn-search'),
		closeCtrl = document.getElementById('btn-search-close'),
		searchContainer = document.querySelector('.search'),
		inputSearch = searchContainer.querySelector('.search__input');

    var searchTerm = "";

    function init() {
        initEvents();
    }

    function initEvents() {
        openCtrl.addEventListener('click', openSearch);
        closeCtrl.addEventListener('click', closeSearch);
        document.addEventListener('keyup', function (ev) {
            // escape key.
            if (ev.keyCode == 27) {
                closeSearch();
            }
        });
    }

    function openSearch() {
        mainContainer.classList.add('main-wrap--move');
        searchContainer.classList.add('search--open');
        inputSearch.focus();
    }

    function closeSearch() {
        mainContainer.classList.remove('main-wrap--move');
        searchContainer.classList.remove('search--open');
        inputSearch.blur();
        inputSearch.value = '';
    }

    $("#searchContent").keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();     //<---- Para evitar el reload de la pagina al apretar Enter (encuentre o no un resultado)
            var searchTerm = $('#searchContent').val();
            if (searchTerm != "") {
                searchContent()
            }
        }
        
    });

    function searchContent() {
        var searchTerm = $('#searchContent').val();
            var data = {
                searchTerm: searchTerm
            };
            $.ajax({
                url: '/Search/Search',
                type: 'POST',
                data: JSON.stringify(data),//{ 'searchTerm': searchTerm },
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    var url = result.url + '?searchTerm=' + JSON.stringify(result.searchTerm);
                    window.location.href = url
                },
                error: function (result) {
                }
            });
    }

    init();

})(window);