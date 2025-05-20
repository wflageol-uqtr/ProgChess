import {
  createContext,
  useContext,
  useEffect,
  useLayoutEffect,
  useMemo,
  useState,
} from "react";
import axios from "axios";
import { api } from "../utils/api";

interface AuthContextType {
  token: string | null;
}

const AuthContext = createContext<AuthContextType>();

// Refactor
const AuthProvider = ({ children }: any) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("accessToken")
  );

  useEffect(() => {
    const verifyToken = async () => {
      try {
        // Http-only cookie so what ?
        const response = await api.get(
          "http://localhost:5290/api/auth/verify-token"
        );
        console.log(response);

        setToken(localStorage.getItem("accessToken"));
      } catch (error) {
        console.log("pas bon");

        setToken(null);
      }
    };

    if (token) {
      verifyToken();
    }
  }, []);

  useLayoutEffect(() => {
    if (token) {
      api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    } else {
      api.defaults.headers.common["Authorization"];
    }
  }, []);

  useLayoutEffect(() => {
    const refreshInterceptor = api.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;

        if (error.response.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true; // Mark the request as retried to avoid infinite loops.
          try {
            const refreshToken = localStorage.getItem("refreshToken"); // Retrieve the stored refresh token.
            // Make a request to your auth server to refresh the token.
            const response = await axios.post(
              "http://localhost:5290/api/auth/refresh-token",
              {
                Id: 1,
                refreshToken,
              }
            );
            const { accessToken, refreshToken: newRefreshToken } =
              response.data;
            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", newRefreshToken);
            api.defaults.headers.common[
              "Authorization"
            ] = `Bearer ${accessToken}`;
            return api(originalRequest);
          } catch (refreshError) {
            console.error("Token refresh failed:", refreshError);
            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");
            window.location.href = "/login";
            return Promise.reject(refreshError);
          }
        }
        return Promise.reject(error); // For all other errors, return the error as is.
      }
    );

    // const refreshInterceptor = axios.interceptors.response.use(
    //   (response) => response,
    //   async (error) => {
    //     if (error.response.status == 401 && !refreshCall) {
    //       try {
    //         console.log("refresh Token");
    //         console.log(localStorage.getItem("refreshToken"));
    //         setRefreshCall(true);
    //         console.log(refreshCall);

    //         const response = await axios.post(
    //           "http://localhost:5290/api/auth/refresh-token",
    //           { id: 1, refreshToken: localStorage.getItem("refreshToken") },
    //           { withCredentials: true }
    //         );
    //         console.log("1");

    //         console.log(response);
    //         console.log("2");

    //         localStorage.setItem("accessToken", response.data.accessToken);
    //         localStorage.setItem("refreshToken", response.data.refreshToken);
    //         setToken(response.data.accessToken);

    //         error.config.headers["Authorization"] =
    //           "Bearer " + response.data.accessToken;
    //         return axios.request(error.config);
    //       } catch (refreshError) {
    //         console.error("Refresh failed:", refreshError);

    //         setToken(null);
    //         return Promise.reject(refreshError);
    //       }
    //     }
    //     return Promise.reject(error);
    //   }
    // );

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
