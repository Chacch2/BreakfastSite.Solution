$(document).ready(function () {
    var rankList = [];
    var currentPoints = 0;
    var currentPage = 1;
    var rowsPerPage = 5;
    var filteredArray = [];

    // 从 p.json 获取数据
    $.getJSON("/html/json/p.json", function (data) {
        rankList = data.rankList;
        currentPoints = data.currentPoints;

        // 更新剩餘點數显示
        $(".points").text(currentPoints + " 點");

        filteredArray = rankList; // 初始化过滤数组
        updateTable();
        updatePagination();
    });

    // 初始化日期选择器，不使用 onSelect 回调
    $("#datepicker").datepicker({
        dateFormat: "yy-mm-dd", // 日期格式设定
    });

    // 为搜索按钮添加点击事件
    $(document).on("click", ".search-btn", function () {
        var selectedDate = $("#datepicker").val();
        if (selectedDate) {
            currentPage = 1; // 重置为第一页
            filterByDate(selectedDate); // 根据选择的日期进行筛选
        } else {
            alert("請選擇日期");
        }
    });

    // 日期过滤函数
    function filterByDate(dateText) {
        filteredArray = rankList.filter(function (order) {
            // 直接比较日期，不进行格式转换
            var getDate = order.Date.split(" ")[0]; // 提取订单日期部分
            return getDate === dateText;
        });

        currentPage = 1; // 重置为第一页
        updateTable();
        updatePagination();
    }

    // 更新表格函数
    function updateTable() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        const rowsToDisplay = filteredArray.slice(start, end);

        const tableBody = $("#table-body");
        tableBody.empty(); // 清空表格

        if (rowsToDisplay.length === 0) {
            tableBody.append(`<tr><td colspan="6">沒有資料</td></tr>`);
        } else {
            rowsToDisplay.forEach(function (order) {
                tableBody.append(`
          <tr>
            <td>${order.OrderID}</td>
            <td>${order.Date}</td>
            <td>${order.TotalAmount}</td>
            <td class="earned-points">${order.EarnedPoints} 點</td>
            <td class="used-points">${order.UsedPoints} 點</td>
            <td>${order.RemainingPoints}</td>
          </tr>
        `);
            });
        }
    }

    // 更新分页函数
    function updatePagination() {
        const totalPages = Math.ceil(filteredArray.length / rowsPerPage);
        const pagination = $("#pagination");

        // 清空分页
        pagination.empty();

        if (totalPages > 1) {
            // 添加上一页按钮
            pagination.append(`<button class="prev-btn">&lt;</button>`);
        }

        // 添加页码按钮
        for (let i = 1; i <= totalPages; i++) {
            const activeClass = i === currentPage ? "active" : "";
            pagination.append(
                `<button class="page-btn ${activeClass}">${i}</button>`
            );
        }

        if (totalPages > 1) {
            // 添加下一页按钮
            pagination.append(`<button class="next-btn">&gt;</button>`);
        }

        // 处理点击事件
        $(".page-btn").click(function () {
            currentPage = parseInt($(this).text());
            updateTable();
            updatePagination();
        });

        $(".prev-btn").click(function () {
            if (currentPage > 1) {
                currentPage--;
                updateTable();
                updatePagination();
            }
        });

        $(".next-btn").click(function () {
            if (currentPage < totalPages) {
                currentPage++;
                updateTable();
                updatePagination();
            }
        });

        // 如果只有一页时隐藏分页
        if (totalPages <= 1) {
            pagination.hide(); // 隐藏分页
        } else {
            pagination.show(); // 显示分页
        }
    }

    // 处理弹出框显示和关闭
    $("#alert-btn").click(function () {
        // 创建并显示遮罩层
        if (!$('#modalOverlay').length) {
            $('body').append('<div id="modalOverlay"></div>');
            $('#modalOverlay').css({
                position: 'fixed',
                top: 0,
                left: 0,
                width: '100%',
                height: '100%',
                backgroundColor: 'rgba(0, 0, 0, 0.5)', /* 半透明黑色背景 */
                zIndex: 998,
                display: 'none'
            });
        }
        $("#modalOverlay").show(); // 显示遮罩层

        $("#rules-popup").css({ zIndex: 1000 }).show(); // 确保弹出框在遮罩层之上
    });

    $("#close-popup").click(function () {
        $("#rules-popup").hide();
        $("#modalOverlay").hide(); // 隐藏遮罩层
    });
});
