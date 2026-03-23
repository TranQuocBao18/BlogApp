import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const getReadingTime = (content: string): number => {
  const AVG_READING_WPM = 150;

  return Math.ceil(content.split(' ').length / AVG_READING_WPM);
};
