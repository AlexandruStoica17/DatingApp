import { Component, computed, inject, InjectFlags, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes.service';
import { PresenceService } from '../../_services/presence.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  // encapsulation: ViewEncapsulation.None
})
export class MemberCardComponent {
  private likeService = inject(LikesService);
  private PresenceService = inject(PresenceService);
  member = input.required<Member>();
  hasLikded = computed(() => this.likeService.likeIds().includes(this.member().id));
  isOnline = computed(() => this.PresenceService.onlineUsers().includes(this.member().username)); 

  toggleLike(){
    this.likeService.toggleLike(this.member().id).subscribe({
      next: () => {
        if(this.hasLikded()){
          this.likeService.likeIds.update(ids => ids.filter(x => x!== this.member().id))
        }
        else{
          this.likeService.likeIds.update(ids => [...ids, this.member().id])
        }
      }
    })
  }

}
