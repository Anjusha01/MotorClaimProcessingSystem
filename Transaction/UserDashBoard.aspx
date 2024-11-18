<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPages/TransactionPage.Master" AutoEventWireup="true" CodeBehind="UserDashBoard.aspx.cs" Inherits="MotorClaimProcessingSystem.Transaction.UserDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Src/Dashboard.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script>
        function startCounting(elementId, end, interval = 80) {
            let current = 0;
            const counterElement = document.getElementById(elementId);

            const intervalId = setInterval(() => {
                counterElement.textContent = current + " +";
                if (current >= end) {
                    clearInterval(intervalId);
                    counterElement.textContent = end + " ";
                } else {
                    current++;
                }
            }, interval);
        }

        window.onload = function () {
            const currentYear = new Date().getFullYear();
            fetchClaimsData(currentYear);

            startCounting('counter1', totalPolicyCount);
            startCounting('counter2', approvedPolicyCount);
            startCounting('counter3', pendingPolicyCount);
            startCounting('counter4', totalClaim);
        };

        function fetchClaimsData(year) {
            $.ajax({
                url: "UserDashBoard.aspx/GetClaimsData", 
                type: "POST",
                data: JSON.stringify({ year: year }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var claimsData = response.d;
                    console.log(claimsData);

                    renderChart(claimsData.monthlyClaims);
                    renderPieChart(claimsData.totalClaims);
                },
                error: function (error) {
                    console.error("Error fetching claims data:", error);
                }
            });
        }

        function renderPieChart(totalClaimsData) {
            var totalClaimsEstimate = totalClaimsData.reduce((acc, val) => acc + val, 0);

            var ctx = document.getElementById('claimsPieChart').getContext('2d');

            if (window.claimsPieChartInstance) {
                window.claimsPieChartInstance.destroy();
            }

            let currentCount = 0;

          
            const centerTextPlugin = {
                id: 'centerText',
                beforeDraw: function (chart) {
                    const { width } = chart;
                    const ctx = chart.ctx;
                    const text = Math.floor(currentCount);
                    const fontSize = 32;
                    const fontWeight = 'Bold';
                    const fontFamily = 'Cambria';
                    const textColor = 'rgb(52, 86, 118)';
                    const textX = width / 2;
                    const textY = chart.chartArea.top + (chart.chartArea.bottom - chart.chartArea.top) / 2;

                    ctx.save();
                    ctx.font = `${fontSize}px ${fontFamily} ${fontWeight}`;
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';
                    ctx.fillStyle = textColor;
                    ctx.fillText(text, textX, textY);
                    ctx.restore();
                }
            };

           
            window.claimsPieChartInstance = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Open Claim Estimates', 'Closed Claim Estimates', 'Approved Claim Estimates'],
                    datasets: [{
                        data: totalClaimsData,
                        backgroundColor: ['rgba(100,165,224,0.8)', 'rgb(52, 86, 118)', 'rgba(165,165,224)'], // Different colors
                        borderColor: 'rgba(255, 255, 255, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    cutout: '50%',
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        tooltip: {
                            enabled: true,
                        }
                    },
                    animation: {
                        onProgress: function (animation) {
                           
                            currentCount = animation.currentStep / animation.numSteps * totalClaimsEstimate;
                        },
                        onComplete: function () {
                           
                            currentCount = totalClaimsEstimate;
                        }
                    }
                },
                plugins: [centerTextPlugin]
            });
        }


        function renderChart(claimsData) {
            console.log(claimsData);

            var ctx = document.getElementById('claimsChart').getContext('2d');

            if (window.claimsChartInstance) {
                window.claimsChartInstance.destroy();
            }

            window.claimsChartInstance = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                    datasets: [{
                        label: 'Claims per Month',
                        data: claimsData,
                        backgroundColor: 'rgb(52, 86, 118)',
                        borderColor: 'rgba(2,165,224,0.8)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Number of Claims'
                            },
                            ticks: {
                                stepSize: 5
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'Month'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        tooltip: {
                            enabled: true,
                        }
                    }
                }
            });
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="vh-100  text-center  pt-5 dashboard-bg">
        <div class="d-flex justify-content-center align-items-center">
            <%--<h1 class="headings">Welcome to Motor <br /> Insurance Claim System</h1>--%>
            <div class="rows">
                <div class="column">
                    <div class="cards one">
                        <p><i class="fa fa-user" style="color: cornflowerblue;"></i></p>
                        <h3 id="counter1" class="counter">+</h3>
                        <p>Total Policies</p>
                    </div>
                </div>
                <div class="column">
                    <div class="cards one">
                        <p><i class="fa fa-user" style="color: cornflowerblue;"></i></p>
                        <h3 id="counter2" class="counter">+</h3>
                        <p>Approved Policies</p>
                    </div>
                </div>
                <div class="column">
                    <div class="cards one">
                        <p><i class="fa fa-user" style="color: cornflowerblue;"></i></p>
                        <h3 id="counter3" class="counter">+</h3>
                        <p>Pending Policies</p>
                    </div>
                </div>
                <div class="column">
                    <div class="cards one">
                        <p><i class="fa fa-user" style="color: cornflowerblue;"></i></p>
                        <h3 id="counter4" class="counter">+</h3>
                        <p>Total Claims</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="rows">
            <div class="column ">
                <div class="cards two">
                    <canvas id="claimsPieChart" width="300"></canvas>
                </div>
            </div>
            <div class="column ">
                <div class="cards two">
                    <canvas id="claimsChart"></canvas>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
