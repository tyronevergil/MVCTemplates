(function ($, signalR, app) {

    $(function () {

        $(document).ajaxError(function (e, jqXHR) {
            if (jqXHR.status === 401 || jqXHR.status === 403) {
                window.location.replace("/");
            }
        });

        var connection = new signalR.HubConnectionBuilder()
            .withUrl('/datahub')
            .configureLogging(signalR.LogLevel.Information)
            .build();

        var hubConnectedHandlers = [];
        var hubEventHandler = {
            subscribe: function (event, handler) {
                connection.on(event, handler);
            },
            onconnected: function (handler) {
                if (handler && typeof handler === "function")
                    hubConnectedHandlers.push(handler);
            }
        }

        try {
            if (app.__readyHandlers && Array.isArray(app.__readyHandlers)) {
                var appReadyHandlers = app.__readyHandlers;
                delete app.__readyHandlers;
                delete app.ready;

                appReadyHandlers.forEach(function (handler) {
                    handler(hubEventHandler);
                });
            }
        }
        catch (e) {
            console.log(e);
        }

        function startHub() {
            var reconnectHub = function () {
                console.log("Hub error connecting.")
                app.bounce(function () {
                    console.log("Hub reconnecting...")
                    startHub();
                }, 5000)();
            }

            connection.start()
                .then(function () {
                    var deferreds = [];
                    hubConnectedHandlers.forEach(function (handler) {
                        try {
                            deferreds.push(handler());
                        }
                        catch (e) {
                            console.log(e);
                        }
                    });

                    console.log("Hub connected");
                    $.when.apply($, deferreds).done(function () {
                        console.log("App started");
                    });

                    connection.onclose(reconnectHub);
                })
                .catch(reconnectHub);
        }

        startHub();
    });

})(window.jQuery, window.signalR, window.app);
