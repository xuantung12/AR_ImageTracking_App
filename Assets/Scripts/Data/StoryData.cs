using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Story data repository for ar_kid app
/// Contains all pre-loaded stories
/// Ported from Flutter ar_kid app
/// </summary>
public static class StoryData
{
    public static List<Story> GetAllStories()
    {
        return new List<Story>
        {
            new Story(
                id: "1",
                title: "Chú Thỏ Thông Minh",
                author: "Truyện Dân Gian",
                category: "Giáo Dục",
                ageRange: 5,
                summary: "Câu chuyện về chú thỏ thông minh vượt qua thử thách của cáo.",
                content: @"Ngày xửa ngày xưa, trong một khu rừng xanh tươi, có một chú thỏ nhỏ rất thông minh và dũng cảm. Chú sống trong một hang nhỏ dưới gốc cây sồi cổ thụ.

Một hôm, một con cáo gian xảo đến gần hang của chú thỏ và nói: ""Này chú thỏ, tôi biết chú đang ở trong đó. Hãy ra đây và chơi với tôi đi!""

Nhưng chú thỏ thông minh đã nhận ra âm mưu của con cáo. Chú trả lời: ""Anh cáo ơi, em rất muốn ra chơi, nhưng em đang bị ốm. Anh có thể vào trong giúp em không?""

Con cáo nghe vậy liền chui đầu vào hang. Ngay lúc đó, chú thỏ nhanh chóng chạy ra cửa sau và kêu lớn: ""Mọi người ơi, có cáo trong hang của em!""

Các con vật khác trong rừng nghe thấy liền kéo đến. Con cáo hoảng sợ, cố gắng chui ra nhưng bị kẹt trong hang hẹp.

Từ đó, con cáo không bao giờ dám quấy rầy chú thỏ nữa, và chú thỏ được mọi người khen ngợi vì sự thông minh và dũng cảm.

Bài học: Sự thông minh và bình tĩnh có thể giúp chúng ta vượt qua mọi khó khăn."
            ),
            
            new Story(
                id: "2",
                title: "Cô Bé Bán Diêm",
                author: "Hans Christian Andersen",
                category: "Cổ Tích",
                ageRange: 6,
                summary: "Câu chuyện cảm động về cô bé bán diêm trong đêm giao thừa.",
                content: @"Đêm giao thừa lạnh giá, tuyết rơi dày đặc trên những con phố vắng. Một cô bé nhỏ đang run rẩy bước đi, tay cầm một bó diêm để bán.

""Ai mua diêm không ạ? Diêm đây!"" - Cô bé kêu khẽ, nhưng không ai để ý đến cô.

Trời càng tối, càng lạnh. Cô bé tìm đến một góc tường để trú. Cô quyết định đốt một que diêm để sưởi ấm.

Ánh lửa nhỏ bé hiện lên, ấm áp và dịu dàng. Trong ánh lửa ấy, cô bé như thấy một cái lò sưởi lớn, ấm cúng.

Que diêm tắt, cô bé lại đốt một que khác. Lần này cô thấy một mâm cơm thịnh soạn. Nhưng khi cô với tay ra thì que diêm lại tắt.

Cô bé tiếp tục đốt những que diêm, và trong mỗi ánh lửa, cô thấy những điều tuyệt vời: cây thông Noel lung linh, và cuối cùng là bà của cô - người đã luôn yêu thương cô nhất.

""Bà ơi, đừng bỏ cháu! Hãy đưa cháu đi theo bà!"" - Cô bé kêu lên và đốt hết tất cả những que diêm còn lại.

Sáng hôm sau, người ta tìm thấy cô bé nằm im ỉ ở góc tường, với nụ cười trên môi, trong tay còn những que diêm đã cháy.

Bài học: Hãy quan tâm và chia sẻ với những người xung quanh, đặc biệt là những người khó khăn."
            ),
            
            new Story(
                id: "3",
                title: "Kiến và Ve Sầu",
                author: "Truyện Ngụ Ngôn",
                category: "Giáo Dục",
                ageRange: 4,
                summary: "Câu chuyện về sự siêng năng và lười biếng.",
                content: @"Mùa hè nắng nóng, chú kiến nhỏ vẫn chăm chỉ tìm kiếm và chứa thức ăn. Chú đi đi lại lại, khuân từng hạt thóc về tổ.

Còn chị ve sầu thì cứ ngồi trên cành cây, hát líu lo suốt ngày. Chị thấy chú kiến vất vả liền cười:

""Này bạn kiến, trời nóng thế này sao không nghỉ ngơi mà cứ làm việc mãi vậy? Hãy lên đây hát hò với tôi đi!""

Chú kiến đáp: ""Không được đâu bạn ạ! Mùa đông sắp đến, mình phải chuẩn bị thức ăn từ bây giờ!""

""Mùa đông còn lâu lắm! Cứ vui chơi đã!"" - Chị ve sầu lại hát vang.

Thời gian trôi qua, mùa đông đến. Tuyết phủ trắng khắp nơi, lạnh giá buốt.

Chú kiến ở trong tổ ấm áp, có đầy đủ thức ăn. Còn chị ve sầu thì đói lả, lạnh run, không còn chỗ nào kiếm ăn.

Chị đến gõ cửa nhà chú kiến: ""Bạn kiến ơi, cho tôi xin chút thức ăn với!""

Chú kiến nhớ lại mùa hè, chú quyết định giúp đỡ chị ve sầu, nhưng cũng nhắc nhở: ""Lần này mình giúp bạn, nhưng sang năm bạn phải chăm chỉ làm việc nhé!""

Bài học: Hãy chăm chỉ và chuẩn bị cho tương lai, đừng chỉ vui chơi mà quên trách nhiệm."
            ),
            
            new Story(
                id: "4",
                title: "Cậu Bé Chăn Cừu",
                author: "Truyện Ngụ Ngôn",
                category: "Giáo Dục",
                ageRange: 5,
                summary: "Câu chuyện về tầm quan trọng của sự trung thực.",
                content: @"Có một cậu bé chăn cừu trên đồi. Ngày nào cậu cũng ngồi canh đàn cừu ăn cỏ. Công việc rất nhàm chán.

Một hôm, cậu nghĩ ra một trò chơi. Cậu la to: ""Sói đến! Sói đến rồi! Cứu đàn cừu với!""

Người dân trong làng nghe thấy liền chạy ù lên đồi với gậy gộc. Nhưng khi lên đến nơi, họ chỉ thấy cậu bé đang cười khúc khích.

""Không có sói đâu! Chỉ là tôi đùa thôi!"" - Cậu bé nói.

Mọi người giận dữ trở về. Nhưng vài ngày sau, cậu bé lại làm như vậy một lần nữa. ""Sói đến rồi! Giúp tôi với!""

Người dân lại chạy lên, và lại thấy không có sói. Họ càng thêm tức giận.

Rồi một ngày, sói thật sự xuất hiện! Con sói hung dữ lao vào đàn cừu.

Cậu bé hoảng sợ la lớn: ""Sói đến thật rồi! Mọi người ơi cứu tôi!""

Nhưng lần này, không ai tin cậu nữa. Không ai đến giúp. Đàn cừu bị sói tấn công, và cậu bé chỉ biết khóc thương tiếc.

Bài học: Nói dối sẽ làm mất lòng tin của mọi người. Hãy luôn trung thực trong mọi hoàn cảnh."
            ),
            
            new Story(
                id: "5",
                title: "Rùa và Thỏ",
                author: "Truyện Ngụ Ngôn Aesop",
                category: "Giáo Dục",
                ageRange: 4,
                summary: "Cuộc đua giữa rùa chậm chạp và thỏ nhanh nhẹn.",
                content: @"Trong rừng có một chú thỏ rất tự tin về tốc độ của mình. Chú hay khoe khoang và chế giễu chú rùa chậm chạp.

""Này chú rùa, sao bạn chậm thế! Tôi chạy nhanh gấp trăm lần bạn!"" - Chú thỏ cười nhạo.

Chú rùa bình tĩnh đáp: ""Hay là chúng ta thi chạy đua xem ai đến đích trước?""

Chú thỏ cười lớn: ""Được! Tôi sẽ cho bạn thấy tôi nhanh như thế nào!""

Cuộc đua bắt đầu. Chú thỏ phi như bay, chỉ trong chớp mắt đã chạy được nửa đường. Quay lại nhìn, chú thấy rùa còn đang bò chậm chạp.

""Ha ha! Chú rùa còn xa lắm! Mình nghỉ một chút cũng được!"" - Chú thỏ nằm dưới bóng cây và ngủ thiếp đi.

Trong khi đó, chú rùa vẫn kiên trì bò từng bước một, không dừng lại nghỉ ngơi.

Khi chú thỏ tỉnh dậy và chạy đến đích, chú kinh ngạc thấy rùa đã đến nơi rồi!

""Chậm mà chắc vẫn tốt hơn nhanh mà thiếu kiên trì!"" - Chú rùa nói.

Bài học: Sự kiên trì và nỗ lực không ngừng nghỉ quan trọng hơn tài năng thiên bẩm."
            )
        };
    }

    public static List<string> GetCategories()
    {
        return new List<string>
        {
            "Tất Cả",
            "Cổ Tích",
            "Giáo Dục",
            "Phiêu Lưu",
            "Động Vật"
        };
    }
}
