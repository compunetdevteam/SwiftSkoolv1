$(function () {
    var chart = new CanvasJS.Chart("chartContainer", {
        title: {
            text: "@ViewBag.Title"
        },
        animationEnabled: true,
        legend: {
            verticalAlign: "center",
            horizontalAlign: "left",
            fontSize: 15,
            fontFamily: "Helvetica"
        },
        theme: "theme2",
        data: [
            {
                type: "pie",
                indexLabelFontFamily: "Garamond",
                indexLabelFontSize: 15,
                indexLabel: "{label} {y}%",
                startAngle: -20,
                showInLegend: false,
                toolTipContent: "{legendText} {y}%",
                //dataPoints: [
                //    { y: 72.48, legendText: "Google", label: "Google" },
                //    { y: 10.39, legendText: "Bing", label: "Bing" },
                //    { y: 7.78, legendText: "Yahoo!", label: "Yahoo!" },
                //    { y: 7.14, legendText: "Baidu", label: "Baidu" },
                //    { y: 0.22, legendText: "Ask", label: "Ask" },
                //    { y: 0.15, legendText: "AOL", label: "AOL" },
                //    { y: 1.84, legendText: "Others", label: "Others" },
                //{ y: 3.84, legendText: "Facebook", label: "Facebook" }
                //],

                //You can add dynamic data from the controller as shown below. Check the controller and uncomment the line which generates dataPoints.
                dataPoints: @Html.Raw(ViewBag.DataPoints)
                    }
        ]
    });
    chart.render();
});