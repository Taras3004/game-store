import { Component } from "@angular/core";
import { SupportRequestFormComponent } from "../support-request-form/support-request-form.component";
import { BlogDigestComponent } from "../blog-digest/blog-digest.component";
import { TwitterUpdatesComponent } from "../twitter-updates/twitter-updates.component";

@Component({
    selector: "bottom-infobar",
    standalone: true,
    templateUrl: "./bottom-infobar.component.html",
    imports: [
        SupportRequestFormComponent,
        BlogDigestComponent,
        TwitterUpdatesComponent
    ]
})
export class BottomInfobarComponent{

}