(function (app, crypto) {

    app.uuidv4 = function () {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, function (c) {
            return (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16);
        });
    };

    app.once = function () {
        return function (action, timeout) {
            var r = false;
            return function () {
                var ctx = this, args = Array.prototype.slice.call(arguments);
                if (!r) {
                    r = true;

                    +function (fn) {
                        if (timeout) {
                            setTimeout(function () {
                                fn();
                            }, timeout);
                        }
                        else {
                            fn();
                        }
                    }(function () {
                        action.apply(ctx, args);
                    });
                }
            };
        };
    }();

    app.bounce = function () {
        var defaultTimeout = 100;
        return function (action, timeout) {
            return function () {
                var ctx = this, args = Array.prototype.slice.call(arguments);
                setTimeout(function () {
                    action.apply(ctx, args);
                }, timeout || defaultTimeout);
            };
        };
    }();

    app.debounce = function () {
        var defaultTimeout = 100;
        return function (action, timeout, immediate, edge) {
            if (typeof timeout === 'boolean') {
                if (typeof immediate === 'boolean') {
                    edge = immediate;
                }
                immediate = timeout;
                timeout = undefined;
            };

            if (typeof timeout === 'undefined') {
                timeout = defaultTimeout;
            }

            var t, ctx, args;
            return function () {
                ctx = this;
                if (!args || edge) {
                    args = Array.prototype.slice.call(arguments);
                }

                +function (fn) {
                    if (immediate && !t) {
                        fn();
                    }
                    clearTimeout(t);
                    t = setTimeout(function () {
                        t = null;
                        if (!immediate) {
                            fn();
                        }
                    }, timeout);
                }(function () {
                    action.apply(ctx, args);
                    args = undefined;
                });
            };
        };
    }();

    app.__readyHandlers = [];
    app.ready = function (handler) {
        app.__readyHandlers.push(handler);
    };

})(window['app'] || (window['app'] = {}), window.crypto || window.msCrypto);