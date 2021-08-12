

function Purchase(prdId, price, inCart, isChkOut, isSign, prdName) {

    if (isSign == 0) {
        location.assign("/Identity/Account/Login");
    }
    else {
        var qtyId = prdId.toString() + 's';
        var qty = document.getElementById(qtyId).value;
        qty = +qty;
        var prdStr = "err " + prdId.toString()

        $('#' + prdStr).empty();
        //************
        $.get("/Home/GetAvlQty/", { prdId, qty }, function (data) {

            $.each(data, function (index, row) {
                document.getElementById(prdStr).innerHTML = row.qtyTxt
                //--------------------------
                if (row.avl) {
                    if (isChkOut) {
                        location.assign("/Home/ChkOut?prdId=" + prdId + "&qty=" + qty + "&price=" + price + "&isHome=" + true + "&prdName=" + prdName);
                    }
                    else {
                        $.post("/Home/PurchaseOrder/", { prdId, qty, price, inCart, isChkOut, isHome: true });
                        $.ajax({
                            type: "POST",
                            url: "/Home/RenderCart", //in asp.net mvc using ActionResult
                            //  data: data,
                            dataType: 'html',
                            success: function (result) {
                                //Your result is here
                                qty = +qty;
                                $("#crt").html(row.totalQty + qty)
                                //  $('#PrdId').append("<option value='" + row.prdId + "'>" + row.nameId + "</option>")
                            }
                        });
                    }

                }
                //-------------------------
            });

        });
        //*************
    }

}
