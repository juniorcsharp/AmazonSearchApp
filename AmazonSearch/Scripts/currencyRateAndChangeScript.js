function changeCurrency() {

    var YourAppID = "your_openexchangerates_App_Id";
    var currentCurrency = $("#currencyCode").val();
    var convertToCurrency = $("select.currencies option:selected").val();
    var urlForConvertation = "https://openexchangerates.org/api/convert/1/" + currentCurrency + "/" + convertToCurrency + "?app_id=" + YourAppID;
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var result = this.responseText;
            result = JSON.parse(result);
            var currentCurrencyRate = result.response;

            $("#currencyCode").each(function () {

                $("#currencyCode").val(convertToCurrency);

            });

            $("#newPrice").each(function () {

                var newPrice = $("#newPrice").val() * currentCurrencyRate;
                $("#newPrice").val(newPrice);

            });

            $("#usedPrice").each(function () {

                var usedPrice = $("#usedPrice").val() * currentCurrencyRate;
                $("#usedPrice").val(usedPrice);

            });
        }

    };
    xhttp.open("POST", urlForConvertation, true);
    xhttp.send();
};

$(function () {
    var urlForCurrencies = "https://openexchangerates.org/api/currencies.json";
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var result = this.responseText;
            result = JSON.parse(result);
            $.each(result, function (index, value) {
                $("#currencies").append("<option value=" + index + ">[" + index + "] " + value + "</option>")
            });
        }

    };
    xhttp.open("POST", urlForCurrencies, true);
    xhttp.send();
});