
(function () {
    $(function () {
        var tokenUi = '<div class="information-container wrapper" >' +
            '<h2>Authenticate</h2>' +
            '<input placeholder="Username" id="input_username" name="username" type="text" size="25">' +
            '<br/>' + 
            '<input placeholder="Password" id="input_password" name="password" type="password" size="25">' +
            '<br/>' + 
            '<input id="input_authenticate" name="authenticate" type="button" value="Get token">' +
            '<br/><br/>' + 
            '</div>';
        $(tokenUi).insertAfter("#information-container");

        $("#input_authenticate").click(function () {
            var username = $("#input_username").val();
            var password = $("#input_password").val();
            encryptString(username, password);
            
        });
    });

    
    function getToken(username, password) {
        var request = "{ \"UserName\":\"" + username + "\", \"Password\":\"" + password.replace(/\"/g, "") + "\"}";
        $.ajax({
            contentType: "application/json",
            type: "post",
            url: "/api/auth/token",
            dataType: "json",
            data: request,
            success: function (data) {
                
                addAuthorization(data.tokentype + " " + data.token);
            },
            error: function (data) {
                alert(data.responseText);
                $("#input_username").val('');
                $("#input_password").val('');
            }
        });
    };
    function encryptString(username, password) {

        $.ajax({
            contentType: "application/json; charset=utf-8",
            type: "get",
            url: "/api/auth/token/encryptstring" + "/" + password,
            dataType: "text",
            data: "{}",
            success: function (data) {
                getToken(username, data);
            },
            error: function (data) {
                alert(data.responseText);
                $("#input_username").val('');
                $("#input_password").val('');
            }
        });
    };

    function addAuthorization(key) {
        window.swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization", key, "header"));
    };
})();
