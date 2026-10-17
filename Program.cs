/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

using System.Text;

namespace DeviceManagement;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        int now = DateTime.Now.Year;
        DateTime borrowDate =
            new(now, 10, 1);


        //1. KHỞI TẠO THIẾT BỊ VÀ PHÒNG
        PrintHeader(
            "1. KHỞI TẠO THIẾT BỊ VÀ PHÒNG");

        var ac01 = new AirConditionRemote(
            "AC01",
            "Điều khiển điều hòa Daikin",
            now - 6,
            1_000_000m,
            maxBorrowDays: 2,
            lateFeePerDay: 10_000m,
            status: DeviceStatus.UnderMaintenance);

        var mic01 = new Microphone(
            "MIC01",
            "Micro không dây",
            now - 2,
            2_000_000m,
            maxBorrowDays: 1,
            lateFeePerDay: 15_000m);

        var pj01 = new ProjectorRemote(
            "PJ01",
            "Điều khiển máy chiếu Epson",
            now - 1,
            1_500_000m,
            maxBorrowDays: 2,
            lateFeePerDay: 20_000m);


        TeachingDevice[] allDevices =
        {
            ac01,
            mic01,
            pj01
        };

        foreach (TeachingDevice device in allDevices)
            Console.WriteLine(
                $"  {device}");


        var room = new ClassRoom(
            "R301",
            "Phòng dạy 301",
            40);

        foreach (TeachingDevice device in allDevices)
            room.AddDevice(device);

        Console.WriteLine(
            $"  {room}");

        Console.WriteLine(
            $"  Tổng bảo trì: " +
            $"{Device.FormatMoney(room.CalculateAnnualMaintenanceCost())}");


        //2. LUỒNG MƯỢN TRẢ THÀNH CÔNG
        PrintHeader(
            "2. LUỒNG MƯỢN TRẢ THÀNH CÔNG");

        var lecturer = new Lecturer(
            "GV01",
            "Nguyễn Văn An");

        var slip = new BorrowingSlip(
            "PM01",
            lecturer,
            room.RoomName,
            borrowDate);

        slip.AddDevice(
            pj01,
            2);

        slip.AddDevice(
            mic01,
            1);


        decimal projectorFee =
            slip.ReturnDevice(
                "PJ01",
                borrowDate.AddDays(2));

        decimal microphoneFee =
            slip.ReturnDevice(
                "MIC01",
                borrowDate.AddDays(3));


        Console.WriteLine(
            $"  Người mượn: {lecturer}");

        Console.WriteLine(
            $"  PJ01 trả đúng hạn: " +
            $"{Device.FormatMoney(projectorFee)}");

        Console.WriteLine(
            $"  MIC01 trả muộn 2 ngày: " +
            $"{Device.FormatMoney(microphoneFee)}");

        Console.WriteLine(
            $"  Tổng phí trả muộn: " +
            $"{Device.FormatMoney(slip.CalculateTotalLateFee())}");

        //hien thi thiet bi tra muon va chua tra
        List<BorrowItem> lateItems = slip.Items
            .Where(i =>
                i.ReturnedDate.HasValue
                && i.ReturnedDate.Value.Date > i.DueDate.Date)
            .ToList();

        Console.WriteLine(
            $"  Phiếu {slip.SlipId} - thiết bị trả muộn:");

        if (lateItems.Count == 0)
            Console.WriteLine("    Không có");
        else
        {
            foreach (BorrowItem item in lateItems)
                Console.WriteLine(
                    $"    {item.Device.DeviceId} - {item.Device.DeviceName}");
        }

        List<BorrowItem> notReturnedItems = slip.Items
            .Where(i => !i.IsReturned)
            .ToList();

        Console.WriteLine(
            $"  Phiếu {slip.SlipId} - thiết bị chưa trả:");

        if (notReturnedItems.Count == 0)
            Console.WriteLine("    Không có");
        else
        {
            foreach (BorrowItem item in notReturnedItems)
                Console.WriteLine(
                    $"    {item.Device.DeviceId} - {item.Device.DeviceName}");
        }


        //3. KIỂM TRA NGOẠI LỆ
        PrintHeader(
            "3. KIỂM TRA NGOẠI LỆ");

        var ta = new TeachingAssistant(
            "TA01",
            "Trần Minh Bình");


        RunExceptionTest(
            "Mượn thiết bị đang bảo trì",
            () =>
            {
                var testSlip =
                    new BorrowingSlip(
                        "T01",
                        lecturer,
                        room.RoomName,
                        borrowDate);

                testSlip.AddDevice(
                    ac01,
                    1);
            });


        RunExceptionTest(
            "Trợ giảng mượn thiết bị thứ ba",
            () =>
            {
                var d1 = new Microphone(
                    "T11",
                    "Micro 1",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var d2 = new Microphone(
                    "T12",
                    "Micro 2",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var d3 = new Microphone(
                    "T13",
                    "Micro 3",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var testSlip =
                    new BorrowingSlip(
                        "T02",
                        ta,
                        room.RoomName,
                        borrowDate);

                testSlip.AddDevice(d1, 1);
                testSlip.AddDevice(d2, 1);
                testSlip.AddDevice(d3, 1);
            });


        RunExceptionTest(
            "Thêm cùng một micro hai lần",
            () =>
            {
                var d = new Microphone(
                    "T21",
                    "Micro test",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var testSlip =
                    new BorrowingSlip(
                        "T03",
                        lecturer,
                        room.RoomName,
                        borrowDate);

                testSlip.AddDevice(d, 1);
                testSlip.AddDevice(d, 1);
            });


        RunExceptionTest(
            "Mượn micro 3 ngày dù tối đa 1 ngày",
            () =>
            {
                var d = new Microphone(
                    "T31",
                    "Micro test",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var testSlip =
                    new BorrowingSlip(
                        "T04",
                        lecturer,
                        room.RoomName,
                        borrowDate);

                testSlip.AddDevice(
                    d,
                    3);
            });


        RunExceptionTest(
            "Trả thiết bị không thuộc phiếu",
            () =>
            {
                var testSlip =
                    new BorrowingSlip(
                        "T05",
                        lecturer,
                        room.RoomName,
                        borrowDate);

                testSlip.ReturnDevice(
                    "NOTFOUND",
                    borrowDate);
            });


        RunExceptionTest(
            "Trả cùng một thiết bị hai lần",
            () =>
            {
                var d = new ProjectorRemote(
                    "T51",
                    "Remote test",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var testSlip =
                    new BorrowingSlip(
                        "T06",
                        lecturer,
                        room.RoomName,
                        borrowDate);

                testSlip.AddDevice(
                    d,
                    1);

                testSlip.ReturnDevice(
                    d.DeviceId,
                    borrowDate.AddDays(1));

                testSlip.ReturnDevice(
                    d.DeviceId,
                    borrowDate.AddDays(1));
            });


        RunExceptionTest(
            "Ngày trả trước ngày mượn",
            () =>
            {
                var d = new ProjectorRemote(
                    "T61",
                    "Remote test",
                    now,
                    1_000_000m,
                    1,
                    10_000m);

                var testSlip =
                    new BorrowingSlip(
                        "T07",
                        lecturer,
                        room.RoomName,
                        borrowDate);

                testSlip.AddDevice(
                    d,
                    1);

                testSlip.ReturnDevice(
                    d.DeviceId,
                    borrowDate.AddDays(-1));
            });
    }


    private static void RunExceptionTest(
        string title,
        Action action)
    {
        Console.WriteLine(
            $"  {title}:");

        try
        {
            action();

            Console.WriteLine(
                "    Không phát sinh ngoại lệ.");
        }
        catch (BorrowingException ex)
        {
            Console.WriteLine(
                $"    {ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            Console.WriteLine(
                "    Kết thúc kiểm thử.");
        }
    }


    private static void PrintHeader(
        string title)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"===== {title} =====");
    }
}