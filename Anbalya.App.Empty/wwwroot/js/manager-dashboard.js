(function (window, $) {
    const ManagerDashboard = {
        init: function () {
            this.bindSidebarToggle();
            this.renderCharts();
            this.bindFileInputLabels();
        },

        bindSidebarToggle: function () {
            const toggleSelectors = "#sidebarToggle, #sidebarToggleTop";

            $(document).on("click", toggleSelectors, function (evt) {
                evt.preventDefault();
                $("body").toggleClass("sidebar-collapsed");
                $(".sidebar .collapse.show").collapse("hide");
            });

            $(window).on("resize", function () {
                if ($(window).width() < 768) {
                    $(".sidebar .collapse.show").collapse("hide");
                }
            });
        },

        bindFileInputLabels: function () {
            $(document).on("change", ".custom-file-input", function () {
                var files = Array.from(this.files || []);
                if (files.length === 0) {
                    $(this).next(".custom-file-label").text("Choose file");
                } else if (files.length === 1) {
                    $(this).next(".custom-file-label").text(files[0].name);
                } else {
                    $(this).next(".custom-file-label").text(files.length + " files selected");
                }
            });
        },

        renderCharts: function () {
            this.drawAreaChart("managerAreaChart", [0, 10000, 5000, 15000, 10000, 20000, 15000, 25000, 20000]);
            this.drawDonutChart("managerPieChart", [
                { label: "Direct", value: 55, color: "#4e73df" },
                { label: "Social", value: 30, color: "#1cc88a" },
                { label: "Referral", value: 15, color: "#36b9cc" }
            ]);
        },

        drawAreaChart: function (canvasId, data) {
            const canvas = document.getElementById(canvasId);
            if (!canvas || !canvas.getContext) {
                return;
            }

            const ctx = canvas.getContext("2d");
            const width = canvas.width = canvas.clientWidth || 600;
            const height = canvas.height = canvas.clientHeight || 320;
            const padding = 40;
            const max = Math.max.apply(null, data);
            const min = Math.min.apply(null, data);
            const range = Math.max(max - min, 1);
            const stepX = (width - (padding * 2)) / (data.length - 1);

            ctx.clearRect(0, 0, width, height);

            // background grid
            ctx.strokeStyle = "#eaecf4";
            ctx.lineWidth = 1;
            const gridLines = 5;
            for (let i = 0; i <= gridLines; i++) {
                const y = padding + ((height - padding * 2) / gridLines) * i;
                ctx.beginPath();
                ctx.moveTo(padding, y);
                ctx.lineTo(width - padding, y);
                ctx.stroke();
            }

            // area path
            ctx.beginPath();
            ctx.moveTo(padding, this._projectY(data[0], min, range, height, padding));

            data.forEach((value, index) => {
                const x = padding + stepX * index;
                const y = this._projectY(value, min, range, height, padding);
                ctx.lineTo(x, y);
            });

            ctx.lineTo(width - padding, height - padding);
            ctx.lineTo(padding, height - padding);
            ctx.closePath();

            ctx.fillStyle = "rgba(78, 115, 223, 0.2)";
            ctx.fill();

            // line path
            ctx.beginPath();
            ctx.moveTo(padding, this._projectY(data[0], min, range, height, padding));
            data.forEach((value, index) => {
                const x = padding + stepX * index;
                const y = this._projectY(value, min, range, height, padding);
                ctx.lineTo(x, y);
            });

            ctx.strokeStyle = "#4e73df";
            ctx.lineWidth = 3;
            ctx.stroke();

            // points
            data.forEach((value, index) => {
                const x = padding + stepX * index;
                const y = this._projectY(value, min, range, height, padding);
                ctx.beginPath();
                ctx.arc(x, y, 4, 0, Math.PI * 2);
                ctx.fillStyle = "#4e73df";
                ctx.fill();
                ctx.strokeStyle = "#fff";
                ctx.lineWidth = 2;
                ctx.stroke();
            });
        },

        drawDonutChart: function (canvasId, segments) {
            const canvas = document.getElementById(canvasId);
            if (!canvas || !canvas.getContext) {
                return;
            }

            const ctx = canvas.getContext("2d");
            const width = canvas.width = canvas.clientWidth || 320;
            const height = canvas.height = canvas.clientHeight || 320;
            const centerX = width / 2;
            const centerY = height / 2;
            const radius = Math.min(width, height) / 2 - 10;
            const innerRadius = radius * 0.6;
            const total = segments.reduce((sum, segment) => sum + segment.value, 0) || 1;

            let startAngle = -Math.PI / 2;
            ctx.clearRect(0, 0, width, height);

            segments.forEach(segment => {
                const sliceAngle = (segment.value / total) * Math.PI * 2;
                const endAngle = startAngle + sliceAngle;

                ctx.beginPath();
                ctx.moveTo(centerX, centerY);
                ctx.arc(centerX, centerY, radius, startAngle, endAngle);
                ctx.closePath();
                ctx.fillStyle = segment.color;
                ctx.fill();

                startAngle = endAngle;
            });

            // donut hole
            ctx.beginPath();
            ctx.arc(centerX, centerY, innerRadius, 0, Math.PI * 2);
            ctx.fillStyle = "#fff";
            ctx.fill();
        },

        _projectY: function (value, min, range, height, padding) {
            const normalized = (value - min) / range;
            return height - padding - normalized * (height - padding * 2);
        }
    };

    $(function () {
        ManagerDashboard.init();
    });

    window.ManagerDashboard = ManagerDashboard;
})(window, window.jQuery);
