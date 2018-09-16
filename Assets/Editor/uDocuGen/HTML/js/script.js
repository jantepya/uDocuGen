var cardIDs = "";

    $(document).ready(function(){
    $(".core").hide();

    $(".home_button").click(function(){
        $(".core").fadeOut("slow");
        $(".home").fadeIn("slow");
    });

    $(".files").click(function(){
        cardIDs = "#" + $(this).attr("id").toString();
        console.log(cardIDs);
        $(".core").css("opacity",1);
        $(cardIDs).fadeIn("slow");
        $(".home").fadeOut("slow");
        console.log("called func");
      });
});