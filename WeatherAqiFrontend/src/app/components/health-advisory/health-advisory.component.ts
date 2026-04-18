import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-health-advisory',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="advisory-card">
      <h3>Health Advisory</h3>
      <div class="category-badge" [style.backgroundColor]="categoryColor">
        {{ category }}
      </div>
      <div class="advisory-text">
        <p><strong>Recommendation:</strong> {{ advisory }}</p>
        <p><strong>Sensitive Groups:</strong> {{ sensitiveGroupNote }}</p>
      </div>
    </div>
  `,
  styles: [
    `
      .advisory-card {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        margin-top: 1rem;
      }
      h3 {
        margin-top: 0;
        color: #333;
      }
      .category-badge {
        display: inline-block;
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 20px;
        font-weight: bold;
        margin-bottom: 1rem;
      }
      .advisory-text {
        background: #f9f9f9;
        padding: 1rem;
        border-radius: 4px;
        line-height: 1.6;
      }
      .advisory-text p {
        margin: 0.5rem 0;
        color: #555;
      }
    `,
  ],
})
export class HealthAdvisoryComponent {
  @Input() category: string = '';
  @Input() advisory: string = '';
  @Input() sensitiveGroupNote: string = '';

  categoryColor = '#667eea';

  ngOnInit(): void {
    this.categoryColor = this.getCategoryColor(this.category);
  }

  private getCategoryColor(category: string): string {
    switch (category) {
      case 'Good':
        return '#2ecc71';
      case 'Moderate':
        return '#f39c12';
      case 'Sensitive':
        return '#e74c3c';
      case 'Unhealthy':
        return '#c0392b';
      case 'Very Unhealthy':
        return '#8b0000';
      default:
        return '#667eea';
    }
  }
}
