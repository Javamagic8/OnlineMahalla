ko.bindingHandlers.commaDecFor = {
    init: function (element, valueAccessor) {

        var observable = valueAccessor();
        var interceptor = ko.computed({
            read: function () {
                var value = observable();
                return isNaN(value) || value === null || value === ''
                    ? 0
                    : value.toString().replace('.', ',');
            },
            write: function (newValue) {
                if (typeof newValue === "string") {
                    newValue = newValue.replace(",", ".");
                }
                if (newValue !== observable()) {
                    observable(newValue);
                }
            }
        });
        if (element.tagName == 'INPUT')
            ko.applyBindingsToNode(element, {
                value: interceptor
            });
        else
            ko.applyBindingsToNode(element, {
                text: interceptor
            });
    }
};
ko.bindingHandlers.russionNumber = {
    init: function (element, valueAccessor) {

        var observable = valueAccessor();
        var interceptor = ko.computed({
            read: function () {
                var value = observable();
                return isNaN(value) || value === null || value === ''
                    ? value
                    : decimalToRussian(value);
            },
            write: function (newValue) {

            }
        });
        if (element.tagName == 'INPUT')
            ko.applyBindingsToNode(element, {
                value: interceptor
            });
        else
            ko.applyBindingsToNode(element, {
                text: interceptor
            });
    }
};
ko.bindingHandlers.russionNumber4 = {
    init: function (element, valueAccessor) {

        var observable = valueAccessor();
        var interceptor = ko.computed({
            read: function () {
                var value = observable();
                return isNaN(value) || value === null || value === ''
                    ? value
                    : decimalToRussian4(value);
            },
            write: function (newValue) {

            }
        });
        if (element.tagName == 'INPUT')
            ko.applyBindingsToNode(element, {
                value: interceptor
            });
        else
            ko.applyBindingsToNode(element, {
                text: interceptor
            });
    }
};
ko.bindingHandlers.russionNumberNotNol = {
    init: function (element, valueAccessor) {

        var observable = valueAccessor();
        var interceptor = ko.computed({
            read: function () {
                var value = observable();
                return isNaN(value) || value === null || value === ''
                    ? value
                    : decimalToRussianF(value,0);
            },
            write: function (newValue) {

            }
        });
        if (element.tagName == 'INPUT')
            ko.applyBindingsToNode(element, {
                value: interceptor
            });
        else
            ko.applyBindingsToNode(element, {
                text: interceptor
            });
    }
};
ko.bindingHandlers.selectPicker = {
    after: ['options'],   /* KO 3.0 feature to ensure binding execution order */
    init: function (element, valueAccessor, allBindingsAccessor) {
        var $element = $(element);
        $element.addClass('selectpicker').selectpicker();

        var doRefresh = function () {
            $element.selectpicker('refresh');
        }, subscriptions = [];

        // KO 3 requires subscriptions instead of relying on this binding's update
        // function firing when any other binding on the element is updated.

        // Add them to a subscription array so we can remove them when KO
        // tears down the element.  Otherwise you will have a resource leak.
        var addSubscription = function (bindingKey) {
            var targetObs = allBindingsAccessor.get(bindingKey);

            if (targetObs && ko.isObservable(targetObs)) {
                subscriptions.push(targetObs.subscribe(doRefresh));
            }
        };

        addSubscription('options');
        addSubscription('value');           // Single
        addSubscription('selectedOptions'); // Multiple

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            while (subscriptions.length) {
                subscriptions.pop().dispose();
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
    }
};