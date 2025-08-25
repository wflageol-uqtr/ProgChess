import {
  createContext,
  useContext,
  useEffect,
  useLayoutEffect,
  useMemo,
  useState,
} from "react";
import axios, { type AxiosRequestConfig } from "axios";
import api, { apiUrl } from "../utils/api";
import { handleApiError } from "../utils/apiErrorHandler";
interface AuthContextType {
  token: string | null;
  setToken: React.Dispatch<React.SetStateAction<string | null>>;
}

interface RetryQueueItem {
  resolve: (value?: any) => void;
  reject: (error?: any) => void;
  config: AxiosRequestConfig;
}

const AuthContext = createContext<AuthContextType>();

const AuthProvider = ({ children }: any) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("accessToken")
  );
  const refreshAndRetryQueue: RetryQueueItem[] = [];
  let isRefreshing = false;

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      setToken(accessToken);
    }
  }, []);

  useEffect(() => {
    const verifyToken = async () => {
      try {
        await api.get("/api/auth/verify-token");
        setToken(localStorage.getItem("accessToken"));
      } catch (error) {
        setToken(null);
      }
    };

    if (token) {
      verifyToken();
    }
  }, []);

  useLayoutEffect(() => {
    const refreshInterceptor = api.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
          if (!isRefreshing) {
            originalRequest._retry = true;
            isRefreshing = true;
            try {
              const refreshToken = localStorage.getItem("refreshToken");
              const response = await axios.post(
                `${apiUrl}/api/auth/refresh-token`,
                {
                  userId: localStorage.getItem("user"),
                  refreshToken,
                }
              );
              const { accessToken, refreshToken: newRefreshToken } =
                response.data;
              localStorage.setItem("accessToken", accessToken);
              localStorage.setItem("refreshToken", newRefreshToken);

              originalRequest.headers.Authorization = `Bearer ${accessToken}`;
              setToken(accessToken);

              refreshAndRetryQueue.forEach(({ config, resolve, reject }) => {
                api
                  .request(config)
                  .then((response) => resolve(response))
                  .catch((err) => reject(err));
              });

              refreshAndRetryQueue.length = 0;

              return axios(originalRequest);
            } catch (refreshError) {
              handleApiError(error);
              localStorage.removeItem("accessToken");
              localStorage.removeItem("refreshToken");
              localStorage.removeItem("user");
              window.location.href = "/admin/login";
              return Promise.reject(refreshError);
            } finally {
              isRefreshing = false;
            }
          }
          return new Promise<void>((resolve, reject) => {
            refreshAndRetryQueue.push({
              config: originalRequest,
              resolve,
              reject,
            });
          });
        }
        return Promise.reject(error);
      }
    );

    return () => {
      api.interceptors.response.eject(refreshInterceptor);
    };
  }, []);

  const contextValue = useMemo(
    () => ({
      token,
      setToken,
    }),
    [token]
  );

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export default AuthProvider;
