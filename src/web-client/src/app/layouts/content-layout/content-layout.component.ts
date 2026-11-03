import { Component } from "@angular/core";
import { TopNavbarComponent } from "../../shared/components/top-navbar/top-navbar.component";
import { BottomInfobarComponent } from "../../shared/components/bottom-infobar/bottom-infobar.component";
import { RouterOutlet } from "@angular/router";

@Component({
    templateUrl: "./content-layout.component.html",
    imports: [TopNavbarComponent, BottomInfobarComponent, RouterOutlet]
})
export class ContentLayoutComponent{

}