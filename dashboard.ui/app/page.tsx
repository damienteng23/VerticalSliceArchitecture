"use client"

import React, { useState } from "react";
import { 
  Box, Drawer, AppBar, Toolbar, IconButton, Typography, 
  List, ListItem, ListItemButton, ListItemText, ListSubheader, Avatar, Badge, CssBaseline, 
  ThemeProvider
} from "@mui/material";
import { LuMenu } from "react-icons/lu";
import { HiOutlineBell } from "react-icons/hi";
import { theme } from "./_shared/theme";

const drawerWidth = 256;

export default function DashboardPage({ children }: { children: React.ReactNode }) {
  const [mobileOpen, setMobileOpen] = useState(false);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const drawerContent = (
    <Box sx={{ overflow: 'auto' }}>
      <List subheader={<ListSubheader>Analytics</ListSubheader>}>
        <ListItemButton selected>
            <ListItemText primary="Heatmaps" />
          </ListItemButton>
      </List>
      <List subheader={<ListSubheader sx={{ mt: 2 }}>Subheader 2</ListSubheader>}>
        <ListItemButton>
          <ListItemText primary="Subheader 2 item" />
        </ListItemButton>
      </List>
    </Box>
  );

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Box sx={{ display: "flex" }}>
        
        <AppBar>
          <Toolbar>
            <IconButton
              color="inherit"
              onClick={handleDrawerToggle}
              sx={{ mr: 2, display: { lg: "none" } }}
            >
              <LuMenu />
            </IconButton>
            
            <Typography variant="h6" noWrap sx={{ flexGrow: 1, fontWeight: 'bold' }}>
              Dashboard.UI
            </Typography>

            <Box sx={{ display: 'flex', gap: 1, alignItems: 'center' }}>
              <IconButton color="inherit">
                <Badge variant="dot" color="primary">
                  <HiOutlineBell size={30} />
                </Badge>
              </IconButton>
              <Avatar sx={{ width: 32, height: 32, bgcolor: 'neutral.main', fontSize: '0.875rem' }}>
                DT
              </Avatar>
            </Box>
          </Toolbar>
        </AppBar>

        {/* --- SIDEBAR (DRAWER) --- */}
        <Box component="nav" sx={{ width: { lg: drawerWidth }, flexShrink: { lg: 0 } }}>

          {/* --- Mobile View --- */}
          <Drawer
            variant="temporary"
            open={mobileOpen}
            onClose={handleDrawerToggle}
            ModalProps={{ keepMounted: true }}
          >
            <Toolbar />
            {drawerContent}
          </Drawer>

          {/* --- Desktop View --- */}
          <Drawer variant="permanent">
            <Toolbar />
            {drawerContent}
          </Drawer>
        </Box>

        {/* --- MAIN CONTENT --- */}
        <Box component="main" sx={{ flexGrow: 1, p: 3, bgcolor: 'grey.50', minHeight: '100vh' }}>
          <Toolbar /> {/* 3. Offset content so it doesn't hide behind the AppBar */}
          {children}
        </Box>
      </Box>
    </ThemeProvider>
  );
}