function btnPrint_onclick() {
    debugger;
    var ObjBarCodeAx = document.getElementById('ObjBarCode');
    var txtBarCode = document.getElementById('MainContent_txtBarCodeText').value;
    sCommand = BuildCommand(txtBarCode);
    //var sStaticPrinter = "\\\\inhydsateesh\\Zebra  TLP2844";
    var sStaticPrinter = "ZDesigner TLP 2844";
    sPrinter = ObjBarCodeAx.PrintBarCodeCommand(sStaticPrinter, sCommand);
    return false;
}
function BuildCommand(txtBarCode) {
    debugger;
    var rawUrl = window.document.URLUnencoded;
    var obj; var url;
    try {
        obj = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (ex) {
        try {
            obj = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (e) {
            debugger;
    obj = null; } }
    if (obj != null) {
        var strUrl = 'WebScan.aspx?value=' + txtBarCode;
        obj.open("GET", encodeURI(strUrl), false);
        obj.send(null);
        return obj.responseText;
    }
}
