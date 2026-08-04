import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css'],
})
export class SidenavComponent {
  // Menu open/close state
  menuState: { [key: string]: boolean } = {
    dashboard: true,
  };

  toggleMenu(menu: string): void {
    this.menuState[menu] = !this.menuState[menu];
  }

  isMenuOpen(menu: string): boolean {
    return this.menuState[menu] || false;
  }
}
