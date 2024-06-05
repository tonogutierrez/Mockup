const dataSource = {
    chart: {
        caption: "Resultados",
        subcaption: "Esto es una prueba",
        theme: "candy",
        showvalue: "0",
        pointerbghovercolor: "#ffffff",
        pointerbghoveralpha: "80",
        pointerhoverradius: "12",
        showborderonhover: "1",
        pointerborderhovercolor: "#333333",
        pointerborderhoverthickness: "2",
        showtickmarks: "0",
        numbersuffix: "%"
    },
    colorrange: {
        color: [
            {
                minvalue: "0",
                maxvalue: "4",
                label: "Low",
                code: "#e44a00"
            },
            {
                minvalue: "4",
                maxvalue: "7",
                label: "Moderate",
                code: "#f8bd19"
            },
            {
                minvalue: "7",
                maxvalue: "10",
                label: "High",
                code: "#6baa01"
            }
        ]
    },
    pointers: {
        pointer: [
            {
                value: performanceValue,
                tooltext: "Performance del alumno $datavalue  "
            }
        ]
    }
};

FusionCharts.ready(function () {
    var myChart = new FusionCharts({
        type: "hlineargauge",
        renderAt: "chart-container",
        width: "50%",
        height: "50%",
        dataFormat: "json",
        dataSource
    }).render();
});
