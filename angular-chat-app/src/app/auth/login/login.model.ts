export interface UserLoginRepoResponse {
  isSuccess: boolean;
  message: string;
  errorMessage: string[];
  validationMessage: string[];
  token: string;
  username: string;
  email: string;
}
