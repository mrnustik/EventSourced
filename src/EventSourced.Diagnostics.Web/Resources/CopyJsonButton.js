ko.bindingHandlers["CopyJsonButton"] = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        element.onclick = function() {
            const json = ko.unwrap(valueAccessor().Json)
            const el = document.createElement('textarea');
            el.value = json;
            document.body.appendChild(el);
            el.select();
            document.execCommand('copy');
            document.body.removeChild(el);
        };
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

    }
};