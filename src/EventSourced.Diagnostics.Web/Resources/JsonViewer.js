ko.bindingHandlers["JsonViewer"] = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        const json = ko.unwrap(valueAccessor().Json)
        if(!json) return;
        $(element).jsonViewer(JSON.parse(json));
    }
};