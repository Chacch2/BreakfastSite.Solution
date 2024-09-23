function initializejQueryFunctions() {
    $(document).ready(function () {
        var rankList = [];
        var currentPoints = 0;
        var currentPage = 1;
        var rowsPerPage = 5;
        var filteredArray = [];

        // 使用 fetch API 从后端获取数据
        fetch('/api/points/all?memberId=2')  // 使用 GET 方法呼叫 API，传入 memberId 参数
            .then(response => response.json())
            .then(data => {
                console.log("Received data from API:", data); // 打印返回的資料
                rankList = data.rankList; // 获取 RankList 数据

                // 排序 rankList，讓日期最新的排在最前面
                rankList.sort((a, b) => new Date(b.date) - new Date(a.date));

                currentPoints = data.currentPoints; // 获取当前剩余点数

                // 更新剩餘點數显示
                $(".points").text(currentPoints + " 點");

                filteredArray = rankList; // 初始化过滤数组
                updateTable();
                updatePagination();
            })
            .catch(error => {
                console.error('Error fetching data:', error);
                alert('無法從後端獲取資料，請稍後再試');
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
                // 直接比较日期，只提取日期部分，不比較時間
                var getDate = order.date.split("T")[0]; // 提取日期部分
                console.log("提取的日期:", getDate); // 確認日期格式

                return getDate === dateText; // 與選擇的日期進行比較
            });

            //if (filteredArray.length === 0) {
            //    alert("沒有符合條件的訂單");
            //}

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
                    <td>${order.orderID}</td>  <!-- 小寫的orderID -->
                    <td>${order.date}</td>  <!-- 小寫的date -->
                    <td>${order.totalAmount}</td>  <!-- 小寫的totalAmount -->
                    <td class="earned-points">${order.earnedPoints} 點</td>  <!-- 小寫的earnedPoints -->
                    <td class="used-points">${order.usedPoints} 點</td>  <!-- 小寫的usedPoints -->
                    <td>${order.remainingPoints}</td>  <!-- 小寫的remainingPoints -->
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
}
