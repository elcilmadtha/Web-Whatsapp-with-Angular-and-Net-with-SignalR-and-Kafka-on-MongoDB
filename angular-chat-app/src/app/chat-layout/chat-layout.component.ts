import { MediaMatcher } from '@angular/cdk/layout';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, Signal, computed, signal } from '@angular/core';
import { UsersDto } from '../user-list/user-list.model';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ChatWindowService } from '../chat-window/chat-window.service';

@Component({
  selector: 'app-chat-layout',
  templateUrl: './chat-layout.component.html',
  styleUrls: ['./chat-layout.component.scss']
})
export class ChatLayoutComponent implements OnDestroy, OnInit {
  protected readonly isMobile: Signal<boolean>;
  private readonly _mobileQuery: MediaQueryList;
  private readonly _mobileQueryListener: () => void;
  selectedUserDto!: UsersDto;
  currentUserName: string = '';
  constructor(private readonly mediaMatcher: MediaMatcher,
    private readonly cdr: ChangeDetectorRef, private router: Router,
    private readonly authService: AuthService,
    private chatWindowService: ChatWindowService) {
    this._mobileQuery = this.mediaMatcher.matchMedia('(max-width: 600px)');
    const mobile = signal(this._mobileQuery.matches);
    this.isMobile = computed(() => mobile());
    this._mobileQueryListener = () => {
      mobile.set(this._mobileQuery.matches);
      this.cdr.markForCheck(); // optional: to trigger change detection
    };
    this._mobileQuery.addEventListener('change', this._mobileQueryListener);
  }
    ngOnInit(): void {
      this.currentUserName = this.authService.getCurrentUserName();
  }

  ngOnDestroy(): void {
    this._mobileQuery.removeEventListener('change', this._mobileQueryListener);
  }
  selectedUser(event: UsersDto) {
    this.selectedUserDto = event;
  }
  logout(): void {
    this.chatWindowService.resetConnection();
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }
}
