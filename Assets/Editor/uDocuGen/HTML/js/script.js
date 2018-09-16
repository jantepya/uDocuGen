var cardIDs = "";

    $(document).ready(function(){
    $(".core").hide();

    $(".home_button").click(function(){
        $(".core").fadeOut("slow");
        $(".home").fadeIn("slow");
    });

    $(".files").click(function(){
        cardIDs = "#" + $(this).attr("id").toString();
        $(".core").css("opacity",1);
        $(cardIDs).fadeIn("slow");
        $(".home").fadeOut("slow");
        Console.log("called func");
      });
});